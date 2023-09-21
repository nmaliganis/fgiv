using System.ComponentModel.DataAnnotations;

namespace erpl.common.dtos.ResourceParameters.Suspects
{
    public class DeleteSuspectResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
