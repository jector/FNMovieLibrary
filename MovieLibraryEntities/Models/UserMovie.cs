using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibraryEntities.Models
{
    public class UserMovie
    {
        public long Id { get; set; }
        public long Rating { get; set; }
        public DateTime RatedAt { get; set; }
        
        [ForeignKey("UserId")]
        public long UserId { get; set; }
        
        [ForeignKey("MovieId")]
        public long? MovieId { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }

    }
}
