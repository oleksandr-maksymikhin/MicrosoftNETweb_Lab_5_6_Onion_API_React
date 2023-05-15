import React from 'react'
import { useState } from 'react'
import imageToBase64 from 'image-to-base64/browser';

export default function Create() {
  const [movie, setMovie] = useState({ title: '', director: '', poster: '' });
  const [imageUrl, setImageUrl] = useState();

  const handleSelect = (e) => 
  {
    //*************different options to save/present pictures with/without "data:image/png;base64,"
    setImageUrl(URL.createObjectURL(e.target.files[0]));
    //setImageUrl("data:image/png;base64," + URL.createObjectURL(e.target.files[0]));
  }

  const handleSubmit = (event) => 
  {
    //console.log(movie);
    let base64 = "";
    imageToBase64(imageUrl)
      .then((response) => 
      {
        //base64 = response;
        base64 = "data:image/png;base64," + response;
        //console.log("base64 = " + base64);
        let toSend = movie;
        toSend.poster = base64;
        //console.log("toSend.poster = " + toSend.poster);
        const options = 
        {
          method: 'POST',
          headers: {'Content-Type':'application/json; charset=utf-8'},
          body: JSON.stringify(toSend)
        }

        let url = 'http://localhost:5127/api/v1/movies';

        fetch(url, options)
          .then((response) => 
          {
            console.log(response);
          })
          .catch((error) => 
          {
            console.log(error);
          })

      })
      .catch((error) => 
      {
        console.log(error);
      })
      setMovie({ title: '', director: '', poster: '' });
      setImageUrl('');
  }

  return (
    <div className='row justify-content-center'>
      <h3 className='text-center mt-3'>Add movie</h3>
      <input type='text' name='title' placeholder='Title' value={movie.title} onChange={(e) => setMovie({ ...movie, title: e.target.value })} 
        className='form-control m-2 w-50'/>
      <input type='text' name='director' placeholder='Director' value={movie.director} onChange={(e) => setMovie({ ...movie, director: e.target.value })} 
        className='form-control m-2 w-50'/>
      <input type='file' name='poster' onChange={handleSelect} 
        className='form-control m-2 w-50'/>
      <button className='btn btn-success w-50' onClick={handleSubmit} disabled={!(imageUrl)}>Add movie</button>
    </div>
  )
}
