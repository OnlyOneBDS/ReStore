import { createBrowserHistory } from "history";
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from "react-redux";
import { Router } from 'react-router-dom';

import "slick-carousel/slick/slick-theme.css";
import "slick-carousel/slick/slick.css";
import App from './app/layout/App';
import './app/layout/styles.css';
import { store } from "./app/store/configureStore";
import reportWebVitals from './reportWebVitals';

export const history = createBrowserHistory();

ReactDOM.render(
  <React.StrictMode>
    <Router history={history}>
      <Provider store={store}>
        <App />
      </Provider>
    </Router>
  </React.StrictMode>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
