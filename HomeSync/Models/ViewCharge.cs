using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
	public class ViewCharge
	{
		[Key]
		public int Charge { get; set; }
		
	}
}
