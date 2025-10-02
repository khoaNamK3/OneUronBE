using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ResourceDTOs
{
    public class ResourceResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Organization { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public double Star { get; set; }

        public double Reviews { get; set; }

        public double Price { get; set; }

        public ResourceType Type { get; set; }

        public CourseDetailResponseDto courseDetail{ get; set; }

        public List<AcknowledgeResponseDto>? Acknowledges { get; set; }

        public List<SkillResponseDto> Skills { get; set; }

        public List<InstructorResponseDto>? Instructors { get; set; }

    }
}
