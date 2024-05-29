using Application.Features.Equipments.Commands.Create;
using Application.Features.Equipments.Commands.Delete;
using Application.Features.Equipments.Commands.Update;
using Infrastructure.NewFolder;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : BaseController
    {

        /// <summary>
        /// Добавляет оборудование в ответ на HTTP POST запрос.
        /// </summary>
        /// <param name="createdEquipmentCommand">Команда на создание оборудования</param>
        /// <returns>Соответствующий HTTP ответ в зависимости от результата добавления оборудования</returns>
        [HttpPost]

        public async Task<IActionResult> Add([FromBody] CreatedEquipmentCommand createdEquipmentCommand)
        {
          
         
                CreatedEquipmentResponse result = await Mediator.Send(createdEquipmentCommand);
                return Ok(result);
           
        }

      
    }
}
