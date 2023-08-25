import React, { Component } from 'react';
import axios from 'axios';
export class FetchData extends Component {
  static displayName = FetchData.name;
 
  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };

  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>نام کالا</th>
            <th>دسته کالا</th>
           
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
        
            <tr key={forecast.id}>
              <td>{forecast.title}</td>
              <td>{forecast.categoryTitle}</td>
            
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    var baseURL="https://192.168.1.53:8082/api/FoodStuffApi/GetFoodStuff";
   
    axios.get(baseURL).then((response) => {
      console.log(response.data)
      this.setState({ forecasts: response.data, loading: false });
      
    });
 
  
  }
}
