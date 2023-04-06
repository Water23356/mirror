namespace Mod_Entity
{
    /// <summary>
    /// 属性接口
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// 该属性所属的实体对象
        /// </summary>
        public Entity Owner { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取当前属性状态
        /// </summary>
        /// <returns></returns>
        public object GetStatus();
        /// <summary>
        /// 当从实体移除此组件时调用
        /// </summary>
        public void Destroy();
        /// <summary>
        /// 初始化内容
        /// </summary>
        public void Initialization();
    }
}
