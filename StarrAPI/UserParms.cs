using StarrAPI.AutoMapperHelp;

namespace StarrAPI
{
    public class UserParams : PaginationParams
    {

        public string CurrentUsername { get; set; }
        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 120;
        public string Orderby { get; set; } = "lastActive";
    }
}