using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    public class WindowManager : Singleton
    {
        public static WindowManager Instance => Instance<WindowManager>();
        private WindowManager() { }
        public void OpenNewWindow(Window to, Window from)
        {
            to.Closing += delegate { from.Show(); };
            to.Show();
            from.Hide();
        }

        public void CloseWindowByFrame(Frame frame)
        {
            Window? window = GetWindowByFrame(frame);
            if (window != null)
            {
                // Вы можете использовать window здесь
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
                window.Close();
            }
        }

        public Window? GetWindowByFrame(Frame frame)
        {
            // Получаем родительский элемент Frame
            FrameworkElement? parent = frame.Parent as FrameworkElement;

            // Переходим к родительским элементам, пока не дойдем до окна или пока не достигнем корневого элемента
            while (parent != null && parent is not Window)
            {
                parent = parent.Parent as FrameworkElement;
            }

            // Если parent это окно, то вернём его, иначе вернётся null
            return parent as Window;
        }
    }
}
