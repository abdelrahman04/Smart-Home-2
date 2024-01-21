
using Microsoft.EntityFrameworkCore;
namespace HomeSync.Data
{
    using HomeSync.Models;
	using Microsoft.Data.SqlClient;
	using static System.Runtime.InteropServices.JavaScript.JSType;
	using System.Net.NetworkInformation;
	using Microsoft.EntityFrameworkCore.Metadata.Internal;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Identity.Client;
	using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
	using Microsoft.Extensions.Logging;

	public class DbContextApp : DbContext {
		public DbContextApp()
        {
        }
    
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Assigned_to>()
                    .HasKey(a => new { a.AdminId, a.TaskId, a.UsersId });

                // Other configurations...

                base.OnModelCreating(modelBuilder);
            }

            public DbContextApp(DbContextOptions<DbContextApp> options) : base(options) { }

		    public DbSet<Task>? Task { get; set; }
		    public DbSet<Room>? Room { get; set; }
		    public DbSet<Finance>? Finance { get; set; }
            public DbSet<Users>? Users { get; set; }
            public DbSet<Communication>? Communication { get; set; }
            public DbSet<Assigned_to>? Assigned_to { get; set; }
            public DbSet<Device>? Device { get; set; }
		public DbSet<Events>? Calendar { get; set; }
        public DbSet<ViewCharge>? ViewCharge { get; set; }

		public DbSet<Iddd>? Iddd { get; set; }
		public DbSet<@try> tri { get; set; }

		public DbSet<Admin>? Admin { get; set; }
		public IEnumerable<Finance> ExecuteReceiveMoney(int sendr,string type, decimal amount, string status, DateTime date)
		{
			var sender = new SqlParameter("@sender_id",sendr);
            var typ = new SqlParameter("@type", type);
            var amnt = new SqlParameter("@amount", amount);
            var stats = new SqlParameter("@status", status);
            var dat = new SqlParameter("@date", date);
            Database.ExecuteSqlRaw("EXEC dbo.ReceiveMoney {0} , {1} , {2} , {3} , {4}", sender, typ, amnt, stats, dat);
          	
		
            SaveChanges();
			return Finance.ToList();
		}
		
		public IEnumerable<Finance> ExecutePlanPayment(int senderId,int receiverId,decimal amount, string status, DateTime deadline)
        {
            var sender = new SqlParameter("@sender_id", senderId);
            var receiver = new SqlParameter("@receiver_id", receiverId);
			var amnt = new SqlParameter("@amount", amount);
			var stats = new SqlParameter("@status", status);
			var dead = new SqlParameter("@deadline", deadline);
            Database.ExecuteSqlRaw("EXEC dbo.PlanPayment @sender_id , @receiver_id , @amount , @status , @deadline" , sender,receiver,amnt,stats,dead);
            SaveChanges();
            return Finance.ToList();
		}
        public IEnumerable<Communication> ExecuteSendMessage(int senderId,int receiverId,string title,string content,TimeOnly timeSent,TimeOnly timeReceived){
            var sender = new SqlParameter("@sender_id", senderId);
            var recipient = new SqlParameter("@reciever_id", receiverId);
            var titl = new SqlParameter("@title", title);
            var contnt = new SqlParameter("@content", content);
            var sent = new SqlParameter("@timesent", timeSent);
            var rec = new SqlParameter("@timereceived", timeReceived);
            Database.ExecuteSqlRaw("EXEC dbo.SendMessage @sender_id={0} , @reciever_id={1} , @title={2} , @content={3} , @timesent={4} , @timerecieved={5}", sender,recipient,titl,contnt,sent,rec);
            SaveChanges();
            return Communication.ToList();  
        }
        public IEnumerable<Communication> ExecuteShowMessages(int receiverId , int senderId)
        {
            var sender = new SqlParameter("@sender_id",senderId);
            var receiver = new SqlParameter("@user_id", receiverId);
      
                var result = Communication.FromSqlRaw("EXEC dbo.ShowMessages {0}, {1}", receiver, sender);
				return result.ToList();
        }
	}
    }