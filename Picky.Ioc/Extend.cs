using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Picky.Ioc
{
    public static class Extend
    {
        /// <summary>
        /// 装配插件
        /// </summary>
        /// <param name="target">载体</param>
        public static void Build(this object target)
        {
            replaceInterface(target);
        }

        /// <summary>
        /// 将接口替换为实现
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mapping"></param>
        private static void replaceInterface(object target)
        {
            var t = target.GetType();
            var fields = t.GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fullName = fields[i].FieldType.FullName;
                if (Factory.ContainsDeclare(fullName))
                {

                    var c = Factory.GetInstance(fullName);
                    fields[i].SetValue(target, c);
                }
                var sub = fields[i].GetValue(target);
                replaceInterface(sub);
            }
        }

        /// <summary>
        /// 装配插件
        /// </summary>
        /// <param name="target">载体</param>
        /// <param name="mapping">插件映射</param>
        public static void Build(this object target, Dictionary<string, string> mapping)
        {
            replaceInterface(target, mapping);
        }

        /// <summary>
        /// 将接口替换为实现
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mapping"></param>
        private static void replaceInterface(object target, Dictionary<string, string> mapping)
        {
            var t = target.GetType();
            var fields = t.GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fullName = fields[i].FieldType.FullName;
                if (mapping.ContainsKey(fullName))
                {

                    var c = Factory.GetInstance(fullName, d => { return d == mapping[fullName]; });
                    fields[i].SetValue(target, c);
                }
                var sub = fields[i].GetValue(target);
                replaceInterface(sub, mapping);
            }
        }

        /// <summary>
        /// 装配插件
        /// </summary>
        /// <param name="target">载体</param>
        /// <param name="mapping">插件映射</param>
        /// <param name="generator">插件生成器</param>
        public static void Build(this object target, Dictionary<string, string> mapping, Func<string, object> generator)
        {
            replaceInterface(target, mapping);
        }

        /// <summary>
        /// 将接口替换为实现
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mapping"></param>
        private static void replaceInterface(object target, Dictionary<string, string> mapping, Func<string, object> generator)
        {
            var t = target.GetType();
            var fields = t.GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fullName = fields[i].FieldType.FullName;
                if (mapping.ContainsKey(fullName))
                {
                    var c = generator(fullName);
                    fields[i].SetValue(target, c);
                }
                var sub = fields[i].GetValue(target);
                replaceInterface(sub, mapping);
            }
        }



    }
}
