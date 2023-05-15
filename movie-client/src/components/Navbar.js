import React from 'react'
import { PrimaryNav, MenuLink, Menu, Hamburger, SecondaryNav } from './styled/NavElement'

export const Navbar = () => {
  return (
    <>
        <PrimaryNav>
          <Hamburger />
          <Menu>
              <MenuLink to="/list" activeStyle>Movies</MenuLink>
              <MenuLink to="/create" activeStyle>Administration</MenuLink>
          </Menu>
        </PrimaryNav>
    </>
  )
}
