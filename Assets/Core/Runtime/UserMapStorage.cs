

namespace MC.Core
{
    [System.Serializable]
    public class UserMapStorage
    {
        /// <summary>
        /// MapName会对应地图物理储存地址
        /// </summary>
        public string MapName;

        /// <summary>
        /// 对应MainUI中MapItem序号
        /// </summary>
        public int MapItemID;
    }
}
