import styled from "styled-components"; 
import {FaBars} from 'react-icons/fa'
import { NavLink as Link} from "react-router-dom";

export const PrimaryNav = styled.nav`
    background-color:${ ({ theme}) => theme.colors.header};
    color: ${ ({ theme}) => theme.colors.text};      
    z-index: 14;
    height: 50px;
    display: flex;
    // background: #8bc34a;
    //justify-content: space-between;
    // flex-wrap: wrap;
    // align-content: space-between;
    padding: 0.18rem calc((100vw - 1000px)/2);
    font-size: 2rem;
`
// 0 12 3 45 6 78 9 ab c de f
// extra rule ingerited from PrimaryNav
// export const SecondaryNav = styled(PrimaryNav)`
// background: ${props => props.bg };
// height: 50px;
// font-weight: 900;
// `
export const Menu = styled.div`
//background-color:${ ({ theme}) => theme.colors.header};
//color: ${ ({ theme}) => theme.colors.text}; 
  display: flex;
  //align-items: center;
  justify-content: space-between;
  margin-right: -25px;
  @media screen and (max-width: 768px) {
    display: none;
  }
`
export const MenuLink = styled(Link)`
  color: #fff;
  //display: flex;
  display: block;
  cursor: pointer;
  align-items: center;
  text-decoration: none;
  padding: 0 1.2rem;
  height: 100%;
  &.active {
    //color: #000000;
    font-weight: bold;
  }
  color: ${ ({ theme}) => theme.colors.text};
`
export const Hamburger = styled(FaBars)`
  display: none;
  color: #ffffff;
  @media screen and (max-width: 768px) {
    display: block;
    font-size: 1.9rem;
    top: 0;
    right: 0;
    position: absolute;
    cursor: pointer;
    transform: translate(-100%, 75%);
  }
`
