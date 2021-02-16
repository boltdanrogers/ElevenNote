using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;

        }//end of constructor

        //now we have a method to create a note

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };//end of new Note

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;

            }//end of using


        }//end of method CreateNote


        //and now a method to get notes

        public IEnumerable<NoteListItem> GetNotes()
        {

            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Notes
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new NoteListItem
                        {
                            NoteId = e.NoteId,
                            Title = e.Title,
                            CreatedUtc = e.CreatedUtc

                        }
                        );

                return query.ToArray();

            }//end of using 

        }//end of method GetNotes

        //now we need a get by Id method
        public NoteDetail GetNoteById(int id)
        {
            //start with our using statement

            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == id && e.OwnerId == _userId);
                return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc

                    };//end of new noteDetail


            }//end of using


        }//end of method GetNoteById


        //a method to update an existing note
        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;

            }//end of using




        }//end of method updateNote




    }//end of class NoteService
}
