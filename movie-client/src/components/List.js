import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
import Tickets from './Tickets';
import './styles/List.css';

export default function List() {
  const [movies, setMovies] = useState([]);
  const [active, setActive] = useState("List");
  const [movieId, setMovieId] = useState(0);
  const [image, setImage] = useState();
  
  let url = 'http://localhost:5127/api/v1/movies';

  let fetchData = () => {
    fetch(url)
    .then((response) => response.json())
    .then((data) => {setMovies(data)})
    .catch((error) => {console.log(error)})
  }

  useEffect(() => {
    fetchData();
    setInterval(() => {fetchData()},20000)
  }, [])

  const toTicketsHandler = (m) =>{
    setActive('Tickets'); setMovieId(m.id); setImage(m.poster)
  }

    return (
        <div className="container">
            {active === "List" &&
                <div>
                    {/* <h3 className='text-center'>Movies</h3> */}
                    <div>
                        {
                            Array.isArray(movies) ?
                            <div className="d-flex flex-wrap">
                            {
                                movies.map((m, index) => (
                                    ////my card
                                    <div class="card-item" data-descr={m.title}>
                                        <a id={m.id+"poster"} onClick={() =>{toTicketsHandler(m)}}>
                                            <img class="d-block w-100 h-100" src={m.poster} alt={m.title}/>
                                        </a>
                                    </div>
                                    
                                    ////standard card
                                    // <div key={index}>
                                    //     <img src={m.poster} id={m.id+"poster"}/>
                                    //     <div id={m.id}>
                                    //         <div>
                                    //             <h3>{m.title}</h3>
                                    //             <p><i>{m.director}</i></p>
                                    //         </div>
                                    //     </div>
                                    // </div>
                                ))
                            }
                            </div>
                            : <span>Not array for card</span>
                        }
                        
                        
                        {/* Table-format from CW */}
                        {/* {
                            Array.isArray(movies) ? 
                            <table className='w-100'>
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Title</th>
                                        <th>Director</th>
                                        <th>Poster</th>
                                        <th>Tickets</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        movies.map((m, index) => {
                                            return <tr key={index}>
                                                <td>{m.id}</td>
                                                <td>{m.title}</td>
                                                <td>{m.director}</td>
                                                <td><img src={m.poster} height='100px' alt='poster'/></td>
                                                <td>
                                                    <button className='btn btn-success btn-sm' onClick={ () => {setActive('Tickets'); setMovieId(m.id); setImage(m.poster)}}>ticket</button>
                                                </td>
                                            </tr>
                                        })
                                    }
                                </tbody>
                            </table>
                            : <span>Not array for table</span>
                        } */}
                    </div>
                </div>
            }

            { active === "Tickets" && <Tickets id={movieId} image={image} /> }
        </div>
    )
}
