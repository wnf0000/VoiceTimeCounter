
namespace FormsApp.Service
{
    public interface IServ
    {
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        int GetAppVersion();
        /// <summary>
        /// 获取版本名
        /// </summary>
        /// <returns></returns>
        string GetAppVersionName();
    }
}
