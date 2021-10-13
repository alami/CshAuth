using Microsoft.AspNetCore.Identity;

namespace CodingMilitia.PlayBall.Auth.Web.Data
{
    public class PlayBallUser : IdentityUser
    {
        public string Sample { get; set; }
    }
}