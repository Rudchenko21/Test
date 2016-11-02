using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entities;

namespace Task.DAL.Context
{
    public class GameStoreDbInitializer : DropCreateDatabaseIfModelChanges<GameStoreContext>
    {
        protected override void Seed(GameStoreContext db)
        {
            List<Comment> list = new List<Comment>
            {
                new Comment {Body="First comment",Name="Yaroslav",Comments=new List<Comment> {new Comment { Body="First subcomment",Name="Yaroslav"} } },
                new Comment {Body="Second comment",Name="Alexandr",Comments=new List<Comment> {new Comment { Body="Second comment",Name="Ivan"} } },
                new Comment {Body="Third comment",Name="Yulia" }
            };
            db.Comments.AddRange(list);
            db.SaveChanges();

            Genre first = new Genre { Name = "Action" };
            Genre second = new Genre { Name = "Adventure" };
            Genre third = new Genre { Name = "Races" };
            db.Genres.AddRange(new List<Genre> { first, second, third });
            db.SaveChanges();

            List<Genre> GenreList = new List<Genre>{
                new Genre{Name="FPS",ParentId=1},
                new Genre{Name="TPS",ParentId=1},
                new Genre{Name="Misc",ParentId=1}
            };
            db.Genres.AddRange(GenreList);
            db.SaveChanges();

            List<PlatformType> PlatformTypeList = new List<PlatformType>
            {
                new PlatformType{Name="PC"},
                new PlatformType{Name="Web"},
                new PlatformType{Name="Mobile"}
            };
            db.PlatformTypes.AddRange(PlatformTypeList);
            db.SaveChanges();
            List<Game> GameList = new List<Game>{
                new Game{Name="GTA 5",Key = "GTA_5",Description="GTA5 it's really cool game",Comments=list,Genres=new List<Genre>{GenreList[1]},PlatformTypes=new List<PlatformType>{PlatformTypeList[0],PlatformTypeList[1]}}
            };
            db.Games.AddRange(GameList);
            PlatformTypeList[0].Games = GameList;
            PlatformTypeList[1].Games = GameList;
            db.SaveChanges();
        }

    }
}
