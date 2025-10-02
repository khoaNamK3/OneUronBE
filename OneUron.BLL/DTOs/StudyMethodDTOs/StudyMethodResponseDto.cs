using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.StudyMethodDTOs
{
    public class StudyMethodResponseDto
    {
        public Guid Id { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public Guid MethodId { get; set; }

        public Guid UserId { get; set; }
    }
}
