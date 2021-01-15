import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as SettlementStore from '../../store/settlementNameService';
import { NameCodeSort } from '../../shared/index';

import './SettlementList.scss';
import { stringify } from 'querystring';

// At runtime, Redux will merge together...
type SettlementProps =
    SettlementStore.SettlementState
    & typeof SettlementStore.actionCreators


class FetchData extends React.Component<SettlementProps> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    settlementColumnRef = React.createRef<HTMLDivElement>();
    generatedColumnRef = React.createRef<HTMLDivElement>();

    public settlementSelected(event: React.FormEvent<HTMLButtonElement>) {
        var selectedValue = event.currentTarget.value;
        this.props.setSelectedSettlement(selectedValue);
        this.props.requestSettlementGeneration(() => {
            if (this.settlementColumnRef.current) {
                this.scrollToColumn(this.generatedColumnRef)
            }
        });
    }

    public goToTop(event: React.FormEvent<HTMLButtonElement>) {
        this.scrollToColumn(this.settlementColumnRef)
    }

    public render() {
        return (
            <React.Fragment>
                <div id="generator-row">
                    <div className="col-lg-4 col-12 col-content">{this.renderSettlementTypes()}</div>
                    <div className="col-lg-4 col-12 col-content">{this.renderGeneratedSettlement()}</div>
                    <div className="col-lg-4 col-12 col-content"></div>
                </div>
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestSettlements();
    }

    private renderSettlementTypes() {
        return (
            <div ref={this.settlementColumnRef}>
                <div>
                    <div>
                        <h2>Settlement Types</h2>
                    </div>
                </div>
                <div>
                    {this.props?.settlements.sort((a, b) => a.sortOrder - b.sortOrder).map((settlement: SettlementStore.SettlementType) =>
                        <div className="select-button" key={settlement.code}>
                            <button value={settlement.code} onClick={e => this.settlementSelected(e)}>
                                <div>{settlement.name}</div>
                                <div className="subscript">{settlement.minSize.toLocaleString()} - {settlement.maxSize.toLocaleString()}</div>
                            </button>
                        </div>
                    )}
                </div>
            </div>
        );
    }

    private renderGeneratedSettlement() {
        return (
            <div ref={this.generatedColumnRef} className="flex-magic">
                <div className="d-flex flex-row">
                    <div className="flex-grow-1">
                        <h2>Generated Settlement</h2>
                    </div>
                    <div className="flex-grow-1 align-self-center text-right">
                        <button onClick={e => this.goToTop(e)} className="toTopLink">
                            Back to Top
                 </button>
                    </div>
                </div>
                <div className="col-content">
                    {this.props?.generatedSettlement?.name}
                     | {this.props?.generatedSettlement?.population.toLocaleString()}

                    {this.props?.generatedSettlement?.buildingTypes.map((buildingType: SettlementStore.SettlementBuildingType, index: number) => {
                        return buildingType.buildings.length > 0 ?
                            (
                                <div key={buildingType.name} className="generator-result-card">
                                    <h3>{buildingType.name}</h3>
                                    <blockquote>
                                        {buildingType.buildings.map((building: SettlementStore.SettlementBuilding, index: number) =>
                                            <div key={building.buildingName}>{building.buildingName}</div>
                                        )}
                                    </blockquote>
                                </div>
                            ) : (null)
                        }
                    )}
                </div>
            </div>
        )
    }

    private scrollToColumn(ref: React.RefObject<HTMLDivElement>) {
        if (ref?.current?.parentElement?.parentElement) {
            var offset = ref.current.parentElement.parentElement.offsetTop;
            var top = ref.current.parentElement.offsetTop;
            ref.current.parentElement.parentElement.scrollTo({
                top: top - offset,
                left: 0,
                behavior: 'smooth'
            });
        }
    }
}

export default connect(
    (state: ApplicationState) => state.settlements, // Selects which state properties are merged into the component's props
    SettlementStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
