import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as BuildingStore from '../../store/buildingNameService';

import './BuildingList.scss';

type BuildingProps =
  BuildingStore.BuildingNameState
  & typeof BuildingStore.actionCreators;

class FetchData extends React.Component<BuildingProps> {
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  buildingTypeColumnRef = React.createRef<HTMLDivElement>();
  private renderBuildingTypes() {
    return (
      <div ref={this.buildingTypeColumnRef}>
        <div>
          <div>
            <h2>Building Types</h2>
          </div>
        </div>
        <div>
          {this.props?.buildingTypes.sort((a, b) => a.sortOrder - b.sortOrder).map((buildingType: BuildingStore.BuildingType) =>
            <div className="select-button" key={buildingType.code}>
              <button value={buildingType.code} onClick={e => this.buildingTypeSelected(e)}>{buildingType.name}</button>
            </div>
          )}
        </div>
      </div>
    );
  }

  buildingNameColumnRef = React.createRef<HTMLDivElement>();
  private renderBuildingNames() {
    return (
      <div ref={this.buildingNameColumnRef}>
        <div>
          <div>
            <h2>Building Names</h2>
          </div>
        </div>
        <div>
          {this.props?.buildingNames.map((buildingName: BuildingStore.BuildingName) =>
            <div key={buildingName.buildingName} className="ancestry-name-item">
              <h3>{buildingName.buildingName}</h3>
              <h4>{buildingName.buildingTypeName}</h4>
            </div>
          )}
        </div>
      </div>
    )
  }

  public buildingTypeSelected(event: React.FormEvent<HTMLButtonElement>) {
    var selectedValue = event.currentTarget.value;
    this.props.setSelectedBuildingType(selectedValue);
    this.props.requestBuildingNames(() => {
      //   if (this.optionsColumnRef.current) {
      //     this.scrollToColumn(this.optionsColumnRef)
      //   }
    });
  }

  public render() {
    return (
      <React.Fragment>
        <div id="ancestry-row">
          <div className="col-lg-4 col-12 col-content">{this.renderBuildingTypes()}</div>
          <div className="col-lg-4 col-12 col-content">{this.renderBuildingNames()}</div>
        </div>
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestBuildingTypes();
  }
}

export default connect(
  (state: ApplicationState) => state.buildingNames, // Selects which state properties are merged into the component's props
  BuildingStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);