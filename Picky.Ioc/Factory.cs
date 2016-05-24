using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Picky.Ioc
{
    public class Factory
    {
        /// <summary>
        /// 存放待使用类型
        /// </summary>
        static Dictionary<string, Dictionary<string, Type>> typePool = new Dictionary<string, Dictionary<string, Type>>();

        /// <summary>
        /// 存放单例对象
        /// </summary>
        static Dictionary<string, Dictionary<string, Object>> singlePool = new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="declare">声明</param>
        /// <param name="implement">实现</param>
        public static void Register<T>(Type implement)
        {
            var t = typeof(T);
            var fullName = t.FullName;
            var imFullName = implement.FullName;
            lock (typePool)
            {
                if (!typePool.ContainsKey(fullName))
                    typePool[fullName] = new Dictionary<string, Type>();
                typePool[fullName][imFullName] = implement;
            }
        }

        /// <summary>
        /// 通过dll注册
        /// </summary>
        /// <param name="declare">声明</param>
        /// <param name="filePath">实现地址</param>
        public static void Register<T>(string filePath)
        {
            var t = typeof(T);
            var fullName = t.FullName;
            var ass = Assembly.LoadFile(filePath);
            var types = ass.GetTypes();
            foreach (var type in types)
            {
                //判断是否派生于接口或者基类
                if (type.IsSubclassOf(t) || type.GetInterface(fullName) != null)
                {
                    lock (typePool)
                    {
                        if (!typePool.ContainsKey(fullName))
                            typePool[fullName] = new Dictionary<string, Type>();
                        typePool[fullName][type.FullName] = type;
                    }
                }
            }
        }

        /// <summary>
        /// 通过程序集注册
        /// </summary>
        /// <param name="declare">声明</param>
        /// <param name="ass">程序集</param>
        public static void Register<T>(Assembly ass)
        {
            var t = typeof(T);
            var fullName = t.FullName;
            var types = ass.GetTypes();
            foreach (var type in types)
            {
                //判断是否派生于接口或者基类
                if (type.IsSubclassOf(t) || type.GetInterface(fullName) != null)
                {
                    lock (typePool)
                    {
                        if (!typePool.ContainsKey(fullName))
                            typePool[fullName] = new Dictionary<string, Type>();
                        typePool[fullName][type.FullName] = type;
                    }
                }
            }
        }

        public static bool ContainsDeclare(string declareName)
        {
            return typePool.ContainsKey(declareName);
        }


        /// <summary>
        /// 创建插件对象
        /// </summary>
        /// <param name="declareName">声明</param>
        /// <param name="searchCondition">查询条件</param>
        /// <returns></returns>
        public static object GetInstance(string declareName, Func<string, bool> searchCondition = null)
        {

            if (typePool.ContainsKey(declareName))
            {

                var implements = typePool[declareName];

                if (searchCondition != null)
                {
                    foreach (var item in implements)
                    {
                        if (searchCondition(item.Key))
                            return Activator.CreateInstance(item.Value);
                    }
                }
                else
                    return Activator.CreateInstance(implements.FirstOrDefault().Value);
            }
            else
            {
                throw new NotImplementedException("未找到" + declareName + "的实现！");
            }

            return null;
        }

        public static T GetInstance<T>(Func<string, bool> searchCondition = null)
        {
            var t = default(T);
            var declareName = t.GetType().FullName;

            if (typePool.ContainsKey(declareName))
            {

                var implements = typePool[declareName];

                if (searchCondition != null)
                {
                    foreach (var item in implements)
                    {
                        if (searchCondition(item.Key))
                            return (T)Activator.CreateInstance(item.Value);
                    }
                }
                else
                    return (T)Activator.CreateInstance(implements.FirstOrDefault().Value);
            }
            else
            {
                throw new NotImplementedException("未找到" + declareName + "的实现！");
            }

            return default(T);
        }



    }
}
