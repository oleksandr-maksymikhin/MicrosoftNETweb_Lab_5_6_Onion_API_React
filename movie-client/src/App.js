//import logo from './logo.svg';
import './App.css';
import List from './components/List';
import Create from './components/Create';
import { Navbar } from './components/Navbar';
import { BrowserRouter, Routes, Route} from 'react-router-dom';

import { GlobalStyles } from './components/styled/GlobalStyles';
import { Header} from './components/styled/Header';
import { Footer } from './components/styled/Footer';
import { ThemeButton, ThemeContainer } from './components/styled/ThemeSwitching';
import { light, dark, blue, green} from './components/styled/Theme';
import { useEffect, useState } from 'react';
import { ThemeProvider } from 'styled-components';
//import { Card, CardBody, CardsContainer, CardTitle} from './components/styled/Content';

function App() {
  const [selectedTheme, setSelectedTheme] = useState(light);
  
  const handleThemeChange = (theme) =>{
    setSelectedTheme(theme);
    localStorage.setItem("current-theme", JSON.stringify(theme));
  }
 
  useEffect(() => {
    const readTheme = JSON.parse(localStorage.getItem("current-theme"));
    if (readTheme){
      setSelectedTheme(readTheme);
    }
  }, []);
  
  return (
    //<div>
      <ThemeProvider theme={selectedTheme}>
        <div className="App">
          <GlobalStyles/>
          {/* <Header>Main Page</Header> */}

          <ThemeContainer>
            {/* <span>Select theme:</span> */}
            <ThemeButton className={`light ${selectedTheme === light ? "active" : ""}`} 
              onClick = { () => handleThemeChange(light) }></ThemeButton>
            <ThemeButton className={`dark ${selectedTheme === dark ? "active" : ""}`} 
              onClick = { () => handleThemeChange(dark) }></ThemeButton>
            <ThemeButton className={`blue ${selectedTheme === blue ? "active" : ""}`} 
              onClick = { () => handleThemeChange(blue) }></ThemeButton>
            <ThemeButton className={`green ${selectedTheme === green ? "active" : ""}`}
              onClick = {() => handleThemeChange(green)}></ThemeButton>
          </ThemeContainer>

          {/* <CardsContainer>
            <Card>
              <CardTitle>Chapter 1</CardTitle>
              <CardBody>
                The required metadata to implement SAP Fiori apps can be exported from the SAP Fiori apps reference library. The SAP Best Practices for SAP S/4HANA package currently uses the following three UI add-ons:
              </CardBody>
            </Card>
          </CardsContainer> */}

          {/* <Footer>
            <p>$copy; 2023</p>
          </Footer> */}
        </div>
        {/* </ThemeProvider> */}


      {/* <div className="container"> */}
        <BrowserRouter>
          <Navbar/>
          <Routes>
            <Route path='/list' element={<List />}></Route>
            <Route path='/create' element={<Create />} className='col-md-3 col-lg-3'></Route>
          </Routes>
        </BrowserRouter>
      {/* </div> */}

      <Footer>
            <p>&copy; 2023 STEP-React</p>
      </Footer>

      </ThemeProvider>

    //</div>
    ////Original App.js without menu
    // <div className="container">
    //   <div className='row'>
    //     <div className='col-md-9 col-lg-9'>
    //       <List />
    //     </div>
    //     <div className='col-md-3 col-lg-3'>
    //       <Create />
    //     </div>
    //   </div>
    // </div>
  );
}


export default App;
