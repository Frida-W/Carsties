using System.Drawing;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic;

namespace AuctionService.Controllers;

[ApiController] //check request properties, return bad request if it failed validation, and sent properties to the api endpoint parameters
[Route("api/auctions")] //route to the api endpoint
public class AuctionsController : ControllerBase //
{
   private readonly AuctionDbContext _context;
   private readonly IMapper _mapper;
   public AuctionsController(AuctionDbContext context, IMapper mapper) //inject DB and IMapper interface
   {
      _context = context;   
      _mapper = mapper;
   }

   [HttpGet]
   public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions() //ActionResult: Http response 200, 404
   {
      var auctions = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync(); //将查询结果转换为一个列表 (List<T>) 并以异步方式执行
            
    return _mapper.Map<List<AuctionDto>>(auctions);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
   {
     var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id); //异步检索满足指定条件的第一个记录。如果找不到符合条件的记录，它将返回默认值（通常是 null）。
    
    return _mapper.Map<AuctionDto>(auction);
   }

   [HttpPost]
   public async Task<ActionResult<AuctionDto>> CreateAuction (CreateAuctionDto auctionDto)
   {
    var auction = _mapper.Map<Auction>(auctionDto);
    // TODO: add current user as seller
    auction.Seller = "test";
    _context.Auctions.Add(auction); //这一步还没有存到数据库，ef将追踪所有实体的更改，先存到memory
    var result = await _context.SaveChangesAsync() > 0; //SaveChangesAsync()返回一个int，等于0没有存到数据库，大于0存到数据库
    if (!result) return BadRequest("could not save change to the DB");
    return CreatedAtAction(nameof(GetAuctionById), //CreatedAtAction返回一个201状态码，和一个location头部，指向新创建的资源
        new {auction.Id}, _mapper.Map<AuctionDto>(auction));
   }

   [HttpPut("{id}")]
   public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
   {
        var auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) return NotFound();

        // TODO: CHECK SELLER IS THE CURRENT USER

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("could not save change to the DB");
   }

   [HttpDelete("{id}")]
   public async Task<ActionResult> DeleteAuction(Guid id)
   {
        var auction = await _context.Auctions.FindAsync(id);
        
        if (auction == null) return NotFound();

        //TODO: CHECK SELLER IS THE CURRENT USER

        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();
   
        return BadRequest("could not delete");
}
}