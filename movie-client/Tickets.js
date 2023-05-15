import React from 'react'
import {useEffect} from 'react'

export default function Tickets(props) 
{
  //useEffect(() => {},[])
  useEffect(() => 
  {
    let url = 'http://localhost:5095/api/tickets';
    let sold = [];
        fetch(url)
        .then( (response) => 
            {
                response.json()
                .then((data) => 
                {
                  //console.log(data);
                  data.map((s) => {
                    let item = (s.row -1)*5 + s.seat ;
                    sold.push(item);
                    //console.log(sold);
                    //sold = [...sold, (s.row -1)*5 + s.seat];
                  });
                })
            })
        .catch( (error) => 
            {
                console.log(error)
            })
    //console.log(sold);

    let seats = 20;
    if(document.getElementsByClassName('seat').length == 0)
    {
      for(let i = 0; i < seats; i++)
      {
        let div = document.createElement('div');
        div.innerHTML = i+1;
        console.log(sold);
        console.log(i + "-" + sold.indexOf(i+1));
        if(sold.includes(i+1)){
          div.className = 'selected';
          //console.log('selected');
        }
        else {
          div.className = 'seat';
          //console.log('seat');
        }
       
        document.getElementById('hall').appendChild(div);
      } 
    }
  },[]);
  
  const handlePurchase = (e) => {
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
      let price = 100;
      let ticket = {row: row, seat: seat, price: price, movieId: props.id};
      //console.log(ticket);

      const options = 
        {
          method: 'POST',
          headers: {'Content-Type':'application/json; charset=utf-8'},
          body: JSON.stringify(ticket)
        }

        let url = 'http://localhost:5095/api/tickets';
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
  }

  //<div id="hall" onClick={(e) => {e.target.style.backgroundColor="yellow"}}>
  return (
    <div >
      <div id="hall" onClick={handlePurchase}>
        {/* <div>
          <div className='seat'></div><div className='seat'></div><div className='seat'></div><div className='seat'></div><div className='seat'></div>
        </div> */}
      </div>
      <button className='btn btn-warning' onClick={buyTicket}>Buy tickets</button>
    </div>
    
  )
  
}
