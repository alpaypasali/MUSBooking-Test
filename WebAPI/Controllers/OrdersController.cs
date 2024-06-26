﻿
using Application.Features.Orders.Commands.Create;
using Application.Features.Orders.Commands.Delete;
using Application.Features.Orders.Commands.Update;
using Application.Features.Orders.Queries.GetListWithPage;
using Infrastructure.NewFolder;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        /// <summary>
        /// Добавляет заказ в ответ на HTTP POST запрос.
        /// </summary>
        /// <param name="createdOrderCommand">Команда на создание заказа</param>
        /// <returns>Соответствующий HTTP ответ в зависимости от результата добавления заказа</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOrderCommand createdOrderCommand)
        {

            await Mediator.Send(createdOrderCommand);
            return Ok();

        }

        /// <summary>
        /// Удаляет заказ в ответ на HTTP DELETE запрос.
        /// </summary>
        /// <param name="deletedOrderCommand">Команда на удаление заказа</param>
        /// <returns>Соответствующий HTTP ответ в зависимости от результата удаления заказа</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderCommand deletedOrderCommand)
        {
            await Mediator.Send(deletedOrderCommand);
            return Ok();

        }

        /// <summary>
        /// Обновляет заказ в ответ на HTTP PUT запрос.
        /// </summary>
        /// <param name="updatedOrderCommand">Команда на обновление заказа</param>
        /// <returns>Соответствующий HTTP ответ в зависимости от результата обновления заказа</returns>
        /// 
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOrderCommand updatedOrderCommand)
        {
            await Mediator.Send(updatedOrderCommand);
            return Ok();

        }

        /// <summary>
        /// Получает список заказов в ответ на HTTP GET запрос.
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Список заказов или соответствующий HTTP ответ</returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListOrderQuery query = new() { PageRequest = pageRequest };
            GetListResponse<GetListDto> response = await Mediator.Send(query);
            return Ok(response);


        }
    }
}





