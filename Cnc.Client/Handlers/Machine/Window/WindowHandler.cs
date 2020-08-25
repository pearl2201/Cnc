namespace Cnc.Client.Handlers.Machine.Window
{
    public class WindowHandler
    {
        public string GetCurrentUsername()
        {
            string userName = System.Environment.UserName;           
            return userName;
        }
    }
}