using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EvaluationDTOs
{
    public class EvaluationRequestDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

     
    }
}
