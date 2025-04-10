namespace AlbinMicroService.Libraries.Common
{
    public static class UseComponents
    {
        public static T GetInstanceOf<T>() where T : new()
        {
            return new T();
        }
    }
}
