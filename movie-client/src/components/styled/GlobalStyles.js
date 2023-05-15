import {createGlobalStyle} from "styled-components";
import * as theme from "./Theme";

export const GlobalStyles = createGlobalStyle`
*,
*::before,
*::after{
    margin:0;
    padding:0;
    box-sizing:border-box;
}

body{
    font-family: monospace;
    overflow-x:hidden;
    background-color:${ ({ theme}) => theme.colors.background};
    color:${ ({ theme}) => theme.colors.text};
}

.light {
    background-color:${ theme.light.colors.header};
}

.dark {
    background-color:${ theme.dark.colors.header};
}

.blue {
    background-color:${ theme.blue.colors.header};
}

.green {
    background-color:${ theme.green.colors.header};
}

.active {
    border:2px solid ${ ({ theme}) => theme.colors.border};
}
`;
