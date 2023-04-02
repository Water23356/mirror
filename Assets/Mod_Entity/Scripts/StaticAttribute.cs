namespace Mod_Entity
{
    /// <summary>
    /// 静态属性
    /// </summary>
    public abstract class StaticAttribute : IAttribute
    {
        #region 字段
        protected string attributeName = "静态属性";
        protected Entity owner;
        #endregion

        #region 属性
        public string Name { get => attributeName; set => attributeName = value; }
        public Entity Owner { get => owner; set => owner = value; }
        #endregion

        #region 功能函数
        public void Destroy() { }
        public object GetStatus()
        {
            return null;
        }
        #endregion
    }
}
