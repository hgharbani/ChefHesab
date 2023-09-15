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
      <table className=' table-fixed  border-collapse border border-slate-400 hover:border-collapse overflow-y-scroll' aria-labelledby="tabelLabel">
        <thead className='bg-sky-700'>
          <tr>
            <th className='border border-slate-300'>نام کالا</th>
            <th className='border border-slate-300'>دسته کالا</th>
           
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
        
            <tr key={forecast.id}>
              <td className='border border-slate-300'>{forecast.title}</td>
              <td className='border border-slate-300'>{forecast.categoryTitle}</td>
            
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
