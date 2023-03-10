using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Icon : BaseModel
{
    public string Name { get; set; } = null!;
    public string DestinationUrl { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public byte Order { get; set; }

    #region MyRegion

    /// <summary>
    /// Relation With Page
    /// </summary>
    [ForeignKey("PageId")]
    public Page page { get; set; } = null!;
    public Guid PageId { get; set; }

    #endregion
}