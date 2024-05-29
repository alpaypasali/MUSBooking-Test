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
          
            try
            {
                CreatedEquipmentResponse result = await Mediator.Send(createdEquipmentCommand);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(new { errors = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Удаляет оборудование в ответ на HTTP DELETE запрос.
        /// </summary>
        /// <param name="deleteEquipmentCommand">Команда на удаление оборудования</param>
        /// <returns>Соответствующий HTTP ответ в зависимости от результата удаления оборудования</returns>
        [HttpDelete]

        public async Task<IActionResult> Delete([FromBody] DeleteEquipmentCommand deleteEquipmentCommand)
        {
        
            try
            {
                await Mediator.Send(deleteEquipmentCommand);
                return Ok();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
        /// <summary>
        /// Обновляет оборудование с помощью HTTP PUT запроса.
        /// </summary>
        /// <param name="updatedEquipmentCommand">Команда на обновление оборудования</param>
        /// <returns>Результат обновления оборудования или соответствующий HTTP ответ</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatedEquipmentCommand updatedEquipmentCommand)
        {

            try
            {
                UpdatedEquipmentResponse result = await Mediator.Send(updatedEquipmentCommand);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(new { errors = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}
