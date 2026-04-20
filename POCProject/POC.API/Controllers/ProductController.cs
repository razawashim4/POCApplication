using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using POC.Application.DTO;
using POC.Application.IServices;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    // CREATE Product
    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="model">The product details to create.</param>
    [Route("createProduct")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _service.AddProductAsync(model);
        return Ok(result);
    }

    // GET ALL Product
    /// <summary>
    /// Get all products.
    /// </summary>
    [Route("getAllProducts")]
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(await _service.GetAllProducts());
    }

    // GET ALL Product
    /// <summary>
    /// Get a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    [Route("getProductById/{id}")]
    [HttpGet]
    public async Task<IActionResult> GetProductById(int id)
    {
        return Ok(await _service.GetProductByIdAsync(id));
    }

    // GET ALL Product
    /// <summary>
    /// Delete a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    [Route("deleteProduct/{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteProductAsync([FromRoute]int id)
    {
        var idExist = await _service.GetProductByIdAsync(id);  
        if (idExist.Data == null)
        {
            return NotFound(new APIResponse<ProductDto>
            {
                StatusCode = 404,
                Message = "Product not found",
                Data = null
            });
        }
        return Ok(await _service.DeleteProductById(id));
    }

    // UPDATE Product
    /// <summary>
    /// Update an existing product.
    /// </summary>
    /// <param name="model">The product details to update.</param>
    [Route("updateProducts")]
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var isExist = await _service.GetProductByIdAsync(model.Id);

        if (isExist.Data == null)
        {
            return NotFound(new APIResponse<UpdateProductDto>
            {
                StatusCode = 404,
                Message = "Product not found",
                Data=model
            });
        }
        var updated = await _service.UpdateProductAsync(model);
        return Ok(updated);
    }
}
