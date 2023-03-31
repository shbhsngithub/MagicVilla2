using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI_2ndTime.Model.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }

        public string SpecialDetails { get; set; }

    }
}
