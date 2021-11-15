using System.Collections.Generic;

namespace StreamingMusic.Interfaces
{
    public interface ILocalNotificationsService
    {
        void ShowNotification(string title, string message, IDictionary<string, string> data);
        void HideNotification();
    }
}
