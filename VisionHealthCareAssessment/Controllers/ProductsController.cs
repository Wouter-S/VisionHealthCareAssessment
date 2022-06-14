using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using VisionHealthCareAssessment.Exceptions;
using VisionHealthCareAssessment.Models;
using VisionHealthCareAssessment.Services;

namespace VisionHealthCareAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IImportService _importService;

        public ProductsController(IProductService productService, IImportService importService)
        {
            _productService = productService;
            _importService = importService;
        }


        [HttpGet]
        [Produces(typeof(List<Product>))]
        /// <summary>
        /// Get list of products 
        /// </summary>
        /// <param name="filter">Queries on part of the name</param>
        /// <returns></returns>
        public IActionResult GetProducts([FromQuery] string filter)
        {
            try
            {
                var products = _productService.GetProducts(filter);
                return Ok(products);
            }
            catch (Exception ex)
            {
                //todo, implement logging
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Get product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        [Produces(typeof(Product))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            try
            {
                var products = await _productService.GetProduct(productId);
                return Ok(products);
            }
            catch (ProductNotFoundException)
            {
                //todo, implement logging
                return NotFound();
            }
            catch (Exception ex)
            {
                //todo, implement logging
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Create or update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrUpdateProduct(Product product)
        {
            try
            {
                await _productService.CreateOrUpdateProduct(product);
                return Ok(product);
            }
            catch (ValidationException ex)
            {
                //todo, implement logging
                return BadRequest(ex.ValidationResult.ToString());
            }
        }

        /// <summary>
        /// Delete product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return Ok();
            }
            catch (ProductNotFoundException)
            {
                //todo, implement logging
                return NotFound();
            }
        }

        /// <summary>
        /// Import products from json
        /// </summary>
        /// <returns></returns>
        [HttpPost("import")]
        [Produces(typeof(ImportReport))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportProducts()
        {
            try
            {
                var report = await _importService.Import();
                return Ok(report);
            }
            catch (JsonException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //todo, implement logging
                return BadRequest(ex.Message);
            }
        }
    }
}