using Microsoft.AspNetCore.Mvc;
using pdfood.api.DTO;
using pdfood.api.DTO.Products;
using pdfood.api.Models.Products;
using pdfood.api.Services.Products;
using System.Collections.Generic;

namespace pdfood.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<Pageable<ProductListDTO>> GetAllProducts(
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string? name,
            [FromQuery] string? barCode,
            [FromQuery] string? sort
            )
        {
            var products = _productService.GetAll(limit, offset, name, barCode, sort);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product?> GetProduct(int id)
        {
            var existingProduct = _productService.GetById(id);

            if (existingProduct == null)
            {
                return BadRequest("Produto não encontrado");
            }

            return Ok(existingProduct);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdProduct = await _productService.CreateAsync(productDTO);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductDTO product)
        {
            try
            {
                await _productService.UpdateAsync(id, product);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Product?> DeleteProduct(int id)
        {
            try
            {
                _productService.Delete(id);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
