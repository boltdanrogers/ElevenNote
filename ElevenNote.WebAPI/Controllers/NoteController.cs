using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    public class NoteController : ApiController
    {

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var noteService = new NoteService(userId);
            return noteService;
        
        }//end of method CreateNoteService

        //lets add a get all method
        public IHttpActionResult Get()
        {
            NoteService noteService = CreateNoteService();
            var notes = noteService.GetNotes();
            return Ok(notes);

        }//end of method Get

        public IHttpActionResult Post(NoteCreate note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }//end of if modelState is bad

            var service = CreateNoteService();

            if (!service.CreateNote(note))
            {
                return InternalServerError();
            }//end of if not able to create note

            return Ok();


        }//end of method Post

        //now we need our overloaded get method
        public IHttpActionResult Get(int id)
        {
            NoteService noteService = CreateNoteService();
            var note = noteService.GetNoteById(id);
            return Ok(note);

        }//end of overloaded get method

        //and our PUT method

        public IHttpActionResult Put(NoteEdit note)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }//end of if no valid state

            var service = CreateNoteService();

            if (!service.UpdateNote(note))
            {
                return InternalServerError();
            }//end of if not updated

            //only get here if model state is valid and update note worked

            return Ok();
        }//end of method Put





    }//end of class NoteController
}
