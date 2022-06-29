using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquefiedPetroleumGas.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public string AgencyId { get; set; }
        public string AgentId { get; set; }
        [ForeignKey("AgencyId")]
        public virtual Agencies Agency { get; set; }
        [ForeignKey("AgentId")]
        public virtual Agents Agent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}