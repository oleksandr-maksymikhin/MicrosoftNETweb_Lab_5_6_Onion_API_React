import styled from "styled-components";

export const Footer = styled.footer`
padding: 10px 10px;
text-aligned:center;
background-color:${ ({ theme}) => theme.colors.footer};
color:${ ({ theme}) => theme.colors.background};

a{
    color:${ ({ theme}) => theme.colors.background};
}
position: fixed;
bottom: 0;
width: 100%;
`;