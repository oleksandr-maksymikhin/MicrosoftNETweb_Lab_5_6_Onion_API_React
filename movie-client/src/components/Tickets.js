import React from 'react'
import {useEffect, useState} from 'react'
import List from './List'
import Zoom from 'react-medium-image-zoom'
import 'react-medium-image-zoom/dist/styles.css'
import './styles/Tickets.css';

export default function Tickets(props) 
{
  const [act, setAct] = useState("Tickets")
  const [price, setPrice] = useState();
  //useEffect(() => {},[])
  useEffect(() => 
  {
    //console.log(props.image);
    let sold = [];
    
    let urlGetTicketsList = 'http://localhost:5127/api/v1/movietickets/' + props.id + '/tickets/';
    let ticketNumbersSold = [];

    fetch(urlGetTicketsList)
      .then((response) => response.json())
      .then((data) => new Promise(function(resolve, reject)
      {
        //console.log(data);
        ticketNumbersSold = data;
        console.log(ticketNumbersSold);

        for (let i = 0; i < ticketNumbersSold.length; i++)
        {
          //let seatNumber = getSeatNumber(ticketNumbersSold[i]);
          //sold.push(seatNumber);
          //console.log(ticketNumbersSold[i]);
          let urlGetTiketById = 'http://localhost:5127/api/v1/tickets/' + ticketNumbersSold[i];
          
          // let promis = fetch(urlGetTiketById);
          // let respResult = response.json();
          // let item = parseInt((respResult.row -1)*5 + respResult.seat);
          // console.log(item);
          // sold.push(item);
          // console.log(sold);
          /////////////**************Standard but asyncronous ************
          fetch(urlGetTiketById)
            .then((response) => response.json())
            .then((dataById) =>
            {
              //console.log(data);
              let item = parseInt((dataById.row -1)*5 + dataById.seat) ;
              sold.push(item);
              console.log(sold);
            })
            
          ////////******************Work but in asyncronous mode****************
          // (async () => {
          //   let response = await fetch(urlGetTiketById);
          //   if(response.ok)
          //   {
          //     let respResult = await response.json();
          //     let item = parseInt((respResult.row -1)*5 + respResult.seat);
          //     console.log(item);
          //     sold.push(item);
          //     console.log(sold);
          //   }
          //   else
          //   {
          //     alert("Error in GetTicket" + response.status)
          //   }
          // })();
        }
        
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //need a solution to avoid setTimeout and call next THEN after finishing FETCH in the LOOP
        setTimeout(() => {
          resolve(sold);
        }, 500);
      }
      ))
         .then((sold) => 
         {  
            console.log(sold);
            let seats = 20;
              //seats presentation with grids
              if(document.getElementsByClassName('seat').length === 0)
              {
                for(let i = 0; i < seats; i++)
                {
                  let div = document.createElement('div');
                  div.innerHTML = i+1;
                  //console.log(sold);
                  //console.log(i + "-" + sold.indexOf(i+1));
                  if(sold.includes(i+1)){
                    div.className = 'sold';
                    //console.log('selected');
                  }
                  else {
                    div.className = 'seat';
                    //console.log('seat');
                  }
                  document.getElementById('hall').appendChild(div);
                } 
              } 
      })
      .catch( (error) => {console.log(error)})
  },[]);
  
  //method return array of PROMIS but need array of INT. How to get INT instead of PROMIS?
  async function getSeatNumber(ticketId){
    
    let urlGetTiketById = 'http://localhost:5127/api/v1/tickets/' + ticketId;
    let response = await fetch(urlGetTiketById);
    let ticket = await response.json();
    let seatNumber = parseInt((ticket.row -1)*5 + ticket.seat) ;
    return seatNumber;
  }
  
  
  const handlePurchase = (e) => {
    if(e.target.className=='sold'){
      return};
    e.target.className == 'seat' ? e.target.className = 'selected' : e.target.className = 'seat';
  }
  
  const buyTicket = (e) => 
  {
    let seats = document.getElementsByClassName('selected');
    let selected = [...seats];
    selected.map( (t) => 
    { 
      //console.log(t.innerHTML);
      let id = parseInt(t.innerHTML) - 1;
      let row = parseInt(id / 5) + 1;
      let seat = id % 5 + 1;
      //let price = 100;
      let ticket = {row: row, seat: seat, price: price};
      console.log(ticket);
      let postedTicketId;

      //POST ticket to SQL Server
      let urlSQL = 'http://localhost:5127/api/v1/tickets/';
      const optionsSQL = 
        {
          method: 'POST',
          headers: {'Content-Type':'application/json; charset=utf-8'},
          body: JSON.stringify(ticket)
        }

      fetch(urlSQL, optionsSQL)
      .then((response) => response.json())
      .then((ticketCreated) =>
        {
          postedTicketId = ticketCreated.id;
          console.log('postedTicketId = ' + postedTicketId);
          let child = document.getElementById('hall').childNodes;
          child.forEach(element => 
            {
              if(((ticket.row-1)*5 + ticket.seat) === parseInt(element.innerHTML))
              {
                element.className='sold';
                return;
              }
            });

              //POST movieTicket chank to Blob
              let urlBlob = 'http://localhost:5127/api/v1/movietickets/' + props.id + '/tickets/' + postedTicketId;
              console.log('movieId = ' + props.id);
              console.log('ticketId = ' + postedTicketId);
              let movieTicket = {movieId: props.id, ticketId: postedTicketId};
              console.log('movieTicket = ' + movieTicket);
              const optionsBlob = 
              {
                method: 'POST',
                headers: {'Content-Type':'application/json; charset=utf-8'},
                body: JSON.stringify(movieTicket)
              }
              fetch(urlBlob, optionsBlob);


        })
      .catch((error) => {console.log(error);})



    })
    setPrice('');
  }

  return (
    <div >
      {act === "Tickets" &&
        <div className='row'>
          <div className='col-md-6 col-lg-6 mt-3 d-flex flex-column align-items-center'>
            <div className='bg-secondary w-100 mb-3 text-center text-white'>Screen</div>
            <div className='hall-grid' id="hall" onClick={handlePurchase}></div>
            <input type='text' name='price' placeholder='price' value={price} onChange={(e)=>setPrice(e.target.value)}
              className='form-control w-25 mt-3 mb-3'></input>
            <button className='btn btn-warning w-25 mb-3' onClick={buyTicket}>Buy</button>
            <a className='btn text-info w-50 mb-3' onClick={() => {setAct("List")}}>&larr; Back to movies</a>
          </div>
          
          <div className='col-md-6 col-lg-6 mt-3'>
              <Zoom>
                <img className='img-rounded img-thumbnail' src={props.image} height='600px' alt='Picture'/>
              </Zoom>
          </div>

        </div>
      }
      {
        act === "List" && <List/>
      }  
    </div>
  )
} 