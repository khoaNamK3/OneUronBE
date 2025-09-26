using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MethodDTOs
{
    public class MethodResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MethodType Difficulty { get; set; }

        public string TimeInfo { get; set; }

        public List<MethodProResponseDto> Pros { get; set; }

        public List<MethodConResponseDto> Cons { get; set; }

        public List<TechniqueResponseDto> Techniques { get; set; }

        public List<MethodRuleResponseDto> MethodRules { get; set; }
    }
}
