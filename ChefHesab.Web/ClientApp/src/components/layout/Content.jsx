import React,{Component} from 'react';
import { FetchData } from '../FetchData';

class Contents extends Component {



  render() {
    return (
      <div className="content">
      {this.props.path === '/home' && <h1>صفحه اصلی</h1>}
      {this.props.path === '/FetchData' && <FetchData/>}
    </div>
  
    );
  }
}

export default Contents;