namespace Domain.Models;

public class Page:BaseModel
{
    public string Name { get; set; } = null!;

    #region Rels

    /// <summary>
    /// Relation With IconService
    /// </summary>
    public ICollection<Icon> icons { get; set; } = new List<Icon>();

    #endregion
    
}