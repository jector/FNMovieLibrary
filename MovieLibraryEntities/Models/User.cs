using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibraryEntities.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }       
        public long Age { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }
        
        [ForeignKey("OccupationId")]
        public long OccupationId { get; set; }
        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
