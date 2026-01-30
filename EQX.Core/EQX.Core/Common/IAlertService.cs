namespace EQX.Core.Common
{
    public interface IAlertService 
    {
        AlertModel GetById(int id);
        void ChangeCulture(string culture);
    }
}
