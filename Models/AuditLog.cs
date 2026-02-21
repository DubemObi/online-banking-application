using System.Collections.Generic;

namespace Banking.Models
{
    class AuditLog
    {
        public int Id { get; set; }                
        public User? UserId { get; set; }            
        public string Action { get; set; }          
        public string TableName { get; set; }       
        public int? RecordId { get; set; }          
        public string Details { get; set; }         
        public DateTime CreatedAt { get; set; }     
    }
}