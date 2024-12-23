using appData.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Utils.Enitties
{
    public class SignInResponse
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        public string AccessToken { get; set; } = null!;

        public AccountProfileResponseDTO Profile { get; set; } = null!;
    }
}
