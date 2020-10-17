using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ContosoPets.Api.Data;
using ContosoPets.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ContosoPetsContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ContosoPetsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region 获取所有产品
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns>返回产品列表</returns>
        /// <response code="200">产品列表</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        #endregion

        #region 获取指定产品
        /// <summary>
        /// 获取指定产品
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns>返回指定产品</returns>
        /// <response code="200">找到指定产品</response>
        /// <response code="400">参数错误</response>
        /// <response code="404">找不到产品</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        #endregion

        #region 创建产品
        /// <summary>
        /// 创建产品
        /// </summary>
        /// <remarks>
        /// 提交参数:
        /// 
        ///     POST api/Products
        ///     {
        ///         "produtcName": "string",
        ///         "price": 0
        ///     }
        /// </remarks>
        /// <param name="product">产品参数</param>
        /// <returns>创建后的产品</returns>
        /// <response code="201">返回新创建的产品</response>
        /// <response code="400">参数错误</response>            
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductAddDto product)
        {
            var entity = _mapper.Map<Product>(product);
            _context.Products.Add(entity);
            var dto = _mapper.Map<ProductDto>(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductAsync), new { id = dto.Id }, dto);
        }
        #endregion

        #region 删除产品
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns>返回状态码</returns>
        /// <response code="204">删除成功</response>
        /// <response code="400">参数错误</response>
        /// <response code="404">找不到产品</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region 修改产品信息
        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <remarks>
        /// 例子：
        /// 
        ///     原数据
        ///     {
        ///         "baz": "qux",
        ///         "foo": "bar"
        ///     }
        /// 
        ///     patch参数格式
        ///     [
        ///         {
        ///             { "op": "replace", "path": "/baz", "value": "boo" },
        ///             { "op": "add", "path": "/hello", "value": ["world"] },
        ///             { "op": "remove", "path": "/foo" }
        ///         }
        ///     ]
        /// 
        ///     结果
        ///     {
        ///         "baz": "boo",
        ///         "hello": ["world"]
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">产品Id</param>
        /// <param name="patch">patch参数</param>
        /// <returns>成功返回204，找不到返回404</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProductAsync(
            Guid id,
            JsonPatchDocument<Product> patch)
        {
            /*
             * 原数据
             * {
             *    "baz": "qux",
             *    "foo": "bar"
             * }
             * patch参数格式
             * [
             *    { "op": "replace", "path": "/baz", "value": "boo" },
             *    { "op": "add", "path": "/hello", "value": ["world"] },
             *    { "op": "remove", "path": "/foo" }
             * ]
             *  结果
             * {
             *    "baz": "boo",
             *    "hello": ["world"]
             * } 
             */
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            patch.ApplyTo(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

    }
}
