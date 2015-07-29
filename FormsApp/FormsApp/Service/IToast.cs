
namespace FormsApp.Service
{
    public interface IToast
    {
        void Show(string text, ToastLenght lenght = ToastLenght.Short);
    }

    public enum ToastLenght
    {
        Short,
        Long
    }
}
